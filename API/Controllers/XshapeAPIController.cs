using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XshapeAPI.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using XshapeAPI.Functions;

namespace XshapeAPI.Controllers
{
    [Route("")]
    [ApiController]
    public class XshapeAPIController : ControllerBase
    {
        private readonly xshapesContext _dbcontext;

        public XshapeAPIController(xshapesContext context)
        {
            _dbcontext = context;
        }

        [HttpGet("retrieve/{devKey:length(32)}/{userId:int}")]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInvenItem(int userId, string devKey)
        {
            if (!functions.validDevKey(devKey)) { return Unauthorized("Dev key is invalid"); }

            var list = from user in _dbcontext.Users
                                where user.UserId == userId
                                select user.Inventories

                                .Join(_dbcontext.InventoryItems,
                                UserInven => UserInven.InventoryId,
                                DbInvenItem => DbInvenItem.InventoryId,
                                (UserInven, DbInvenItem) => new { UserInven, DbInvenItem })

                                .Join(_dbcontext.Products,
                                UserInvenDbInvenItem => UserInvenDbInvenItem.DbInvenItem.ProductId,
                                Dbproduct => Dbproduct.ProductId, 
                                (UserInvenDbInvenItem, Dbproduct) => new { UserInvenDbInvenItem, Dbproduct})
                                
                                .Join(_dbcontext.Shapes,
                                UserInvenDbInvenItemDbProduct => UserInvenDbInvenItemDbProduct.UserInvenDbInvenItem.DbInvenItem.ShapeId,
                                DbShapes => DbShapes.ShapeId, 
                                (UserInvenDbInvenItemDbProduct, Dbshape) => new {
                                    name = UserInvenDbInvenItemDbProduct.Dbproduct.Name,
                                    location = UserInvenDbInvenItemDbProduct.UserInvenDbInvenItem.UserInven.Location,
                                    quantity = UserInvenDbInvenItemDbProduct.UserInvenDbInvenItem.DbInvenItem.Quantity,
                                    shape = Dbshape.Name,
                                    minQuantity = UserInvenDbInvenItemDbProduct.UserInvenDbInvenItem.DbInvenItem.MinQuantity
                                });


            return Ok(list);
        }

        [HttpGet("retrive/{UserAPIkey:length(32)}")]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInvenItem(string UserAPIkey)
        {
            var list = from user in _dbcontext.Users
                                where user.ApiKey == UserAPIkey
                                select user.Inventories
                                .Join(_dbcontext.InventoryItems,
                                UserInven => UserInven.InventoryId,
                                DbInvenItem => DbInvenItem.InventoryId,
                                (UserInven, DbInvenItem) => new { UserInven, DbInvenItem })

                                .Join(_dbcontext.Products,
                                UserInvenDbInvenItem => UserInvenDbInvenItem.DbInvenItem.ProductId,
                                Dbproduct => Dbproduct.ProductId,
                                (UserInvenDbInvenItem, Dbproduct) => new { UserInvenDbInvenItem, Dbproduct })

                                .Join(_dbcontext.Shapes,
                                UserInvenDbInvenItemDbProduct => UserInvenDbInvenItemDbProduct.UserInvenDbInvenItem.DbInvenItem.ShapeId,
                                DbShapes => DbShapes.ShapeId,
                                (UserInvenDbInvenItemDbProduct, Dbshape) => new {
                                    name = UserInvenDbInvenItemDbProduct.Dbproduct.Name,
                                    location = UserInvenDbInvenItemDbProduct.UserInvenDbInvenItem.UserInven.Location,
                                    quantity = UserInvenDbInvenItemDbProduct.UserInvenDbInvenItem.DbInvenItem.Quantity,
                                    shape = Dbshape.Name,
                                    minQuantity = UserInvenDbInvenItemDbProduct.UserInvenDbInvenItem.DbInvenItem.MinQuantity
                                });

            return Ok(list);
        }

        [HttpPost("update/")]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> UpdateInvenItem(InventoryItemUpdate item)
        {
            if (!functions.validDevKey(item.devKey)) { return Unauthorized("Dev key is either invalid or missing"); }

            var dbReturn = await _dbcontext.InventoryItems.FindAsync(item.ItemId);

            if (dbReturn == null){return NotFound("Item ID not found in DB");}

            int preQuant = dbReturn.Quantity;
            int minQuant = dbReturn.MinQuantity;

            dbReturn.Quantity = item.Quantity;
            await _dbcontext.SaveChangesAsync();

            if (preQuant < minQuant) { return Ok(dbReturn); }

            if (dbReturn.Quantity < minQuant)
            {
                var ItemProduct = await _dbcontext.Products.FindAsync(dbReturn.ProductId);
                var ItemLocation = await _dbcontext.Inventories.FindAsync(dbReturn.InventoryId);
                var userList = from Inventory in _dbcontext.Inventories
                               where Inventory.InventoryId == dbReturn.InventoryId
                               select Inventory.Users.ToArray();

                if (userList == null || ItemProduct == null || ItemLocation == null)
                {
                    return NotFound("Either User, product name or Item Location is not found");
                }

                var apiKey = Environment.GetEnvironmentVariable("sendGridKey"); ;
                var client = new SendGridClient(apiKey);

                var mailList = "";
                var first = true;

                foreach (User[] users in userList)
                {
                    foreach (User user in users)
                    {
                        var PlainTextContent = "Hi " + user.Name + ",\n\nYour Inventory item " + ItemProduct.Name + " is low: \nItem ID: " + dbReturn.ItemId + "\nProduct Name: " + ItemProduct.Name + "\nQuantity: " + dbReturn.Quantity + "\nLocation: " + ItemLocation.Location + "\n\nauto-generated By Xshapes";

                        var msg = new SendGridMessage()
                        {
                            From = new EmailAddress("20004713@myrp.edu.sg", "Xshapes"),
                            Subject = "Low Inventory",
                            PlainTextContent = PlainTextContent
                        };
                        msg.AddTo(new EmailAddress(user.Email, user.Name));
                        var response = await client.SendEmailAsync(msg);


                        if (!first) { mailList += ", "; } else { first = false; }
                        mailList += user.Name + " ";
                    }
                }

                return Ok("Critical low stock, email sent to " + mailList);
            }

            return Ok(dbReturn);
        }

        [HttpPost("login/")]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> login(UserLogin user)
        {
            if (!functions.validDevKey(user.DevKey)) { return Unauthorized("Dev key is either invalid or missing"); }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            User userDbReturn = (from dbUser in _dbcontext.Users
                                 where user.UserName == dbUser.Username
                                 select dbUser).FirstOrDefault();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (userDbReturn == null) { return NotFound("User not found"); }

            if (functions.CompPassword(userDbReturn.Password, user.Password))
            {
                return Ok(new
                {
                    username = userDbReturn.Username,
                    name = userDbReturn.Name,
                    APIKey = userDbReturn.ApiKey,
                    userID = userDbReturn.UserId,
                    lastLogin = userDbReturn.LastLogin,
                    role = userDbReturn.Role
                });
            }
            else 
            { 
                return Unauthorized("Password Incorrect"); 
            }
        }
    }
}
