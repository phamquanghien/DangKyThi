using Microsoft.AspNetCore.Mvc;
using QuanLyCaThi.Data;

namespace QuanLyCaThi.Models.Process
{
    public class CheckSecurityKey
    {
        private readonly ApplicationDbContext _context;

        public CheckSecurityKey(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CheckSecurity(int i, string sCode)
        {
            bool check = false;
            var model = _context.SecurityCode.ToList();
            if(i == 1) {
                var key = model[0].SecurityKey;
                var checkey = StringProcess.CreateMD5Hash(sCode);
                if(key == checkey) check = true;
            }
            if(i == 2) {
                var key = model[1].SecurityKey;
                var checkey = StringProcess.CreateMD5Hash(sCode);
                if(key == checkey) check = true;
            }
            return check;
        }
    }
}