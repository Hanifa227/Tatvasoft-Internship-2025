using Microsoft.AspNetCore.Http;
using Mission.Entities.context;
using Mission.Entities.Entities;
using Mission.Entities.Models;
using Mission.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Repositories.Repositories
{
    public class LoginRepository(MissionDbContext missionDbContext): ILoginRepository
    {
        private readonly MissionDbContext _missionDbContext = missionDbContext;
        public LoginUserResponseModel login(LoginUserRequestModel model)
        {
            var existingUser = _missionDbContext.Users.Where(x=>x.EmailAddress.ToLower() == model.EmailAddress.ToLower() && !x.IsDeleted).FirstOrDefault();
            if(existingUser == null) {
                return new LoginUserResponseModel() { Message = "Email address Not found" };
            }
            if (existingUser.Password != model.Password)
            {
                return new LoginUserResponseModel() { Message = "Incorrect Password" };
            }
            return new LoginUserResponseModel()
            {
                Id = existingUser.Id,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
                EmailAddress = existingUser.EmailAddress,
                PhoneNumber = existingUser.PhoneNumber,
                UserImage = existingUser.UserImage,
                UserType = existingUser.UserType,
                Message = "Login Successfully"
            };
        }


        public string Register(RegisterUserModel Model)
        {
            var isExist = _missionDbContext.Users.Where(x=>x.EmailAddress == Model.EmailAddress && !x.IsDeleted).FirstOrDefault();
            if (isExist != null) throw new Exception("Email already exist");
            User user = new User()
            {
                FirstName = Model.FirstName,
                LastName = Model.LastName,
                EmailAddress = Model.EmailAddress,
                Password = Model.Password,
                PhoneNumber = Model.PhoneNumber,
                UserType = "user",
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow
            };
            _missionDbContext.Users.Add(user);
            _missionDbContext.SaveChanges();
            return "User Added!";
        }

        public UserDetails LoginUserDetailsById(int id)
        {
            var user = _missionDbContext.Users.Where(x => x.Id == id && !x.IsDeleted).Select(user => new UserDetails()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                EmailAddress = user.EmailAddress,
                UserType = user.UserType,
            }).FirstOrDefault() ?? throw new Exception("user not found");

            return user;
        }

        public string UpdateUser(UserDetails model, string webRootPath)
        {
            // ✅ Check if email already exists for another user
            var isEmailExist = _missionDbContext.Users
                .FirstOrDefault(x => x.EmailAddress == model.EmailAddress && x.Id != model.Id && !x.IsDeleted);

            if (isEmailExist != null)
                throw new Exception("Email already exists");

            // ✅ Get the current user
            var existingUser = _missionDbContext.Users
                .FirstOrDefault(x => x.Id == model.Id && !x.IsDeleted) ?? throw new Exception("User not found");

            string finalImagePath = existingUser.UserImage;

            // ✅ Delete old image if new image is uploaded
            if (model.ProfileImage != null && !string.IsNullOrEmpty(finalImagePath))
            {
                string oldImageFullPath = Path.Combine(webRootPath, finalImagePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (File.Exists(oldImageFullPath))
                {
                    File.Delete(oldImageFullPath);
                }

                // ✅ Save new image
                finalImagePath = SaveImageAsync(model.ProfileImage, "Images", webRootPath);
            }

            // ✅ Update user fields
            existingUser.Id = model.Id;
            existingUser.FirstName = model.FirstName;
            existingUser.LastName = model.LastName;
            existingUser.PhoneNumber = model.PhoneNumber;
            existingUser.EmailAddress = model.EmailAddress;
            existingUser.UserType = model.UserType;
            existingUser.UserImage = finalImagePath;
            existingUser.ModifiedDate = DateTime.UtcNow;

            _missionDbContext.Users.Update(existingUser);
            _missionDbContext.SaveChanges();

            return "User updated!";
        }


        private string SaveImageAsync(IFormFile file, string folderName, string webRootPath)
        {
            if (file == null || file.Length == 0) return null;

            string uploadsFolder = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string fileExtension = Path.GetExtension(file.FileName);
            string fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.UtcNow:yyyyMMddHHmmss}{fileExtension}";
            string fullPath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyToAsync(stream);
            }

            return Path.Combine(folderName, fileName).Replace("\\", "/");
        }
    }
}
