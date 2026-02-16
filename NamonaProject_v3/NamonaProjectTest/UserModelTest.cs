using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NamonaProject_v3_.Model;
using NamonaProject_v3_.Persistance;

namespace NamonaProjectTest
{
    public class UserModelTest
    {
        private readonly UserModel _model;
        private readonly NamonaDbContext _context;

        public UserModelTest()
        {
            _context = DbContextFactory.Create();
            _model = new UserModel(_context);
        }

        [Fact]
        public void ShowUsers_Validate()
        {
            var result = _model.ShowUsers().ToList();

            Assert.NotEmpty(result);
            Assert.All(result, r => Assert.True(r.UserId > 0));
            Assert.All(result, r => Assert.False(string.IsNullOrEmpty(r.UserName)));
            Assert.All(result, r => Assert.False(string.IsNullOrEmpty(r.Password)));
            Assert.All(result, r => Assert.False(string.IsNullOrEmpty(r.Role)));
            Assert.All(result, r => Assert.False(string.IsNullOrEmpty(r.Email)));
            //phone number assertion
        }
    }
}
