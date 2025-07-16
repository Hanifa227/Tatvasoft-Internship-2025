using Microsoft.EntityFrameworkCore;
using Mission.Entities.context;
using Mission.Entities.Entities;
using Mission.Entities.Models.CommonModel;
using Mission.Entities.Models.MissionsModels;
using Mission.Repositories.IRepositories;

namespace Mission.Repositories.Repositories
{
    public class MissionRepository(MissionDbContext dbContext) : IMissionRepository
    {
        private readonly MissionDbContext _dbContext = dbContext;

        public List<DropDownResponseModel> MissionThemeList()
        {
            var missionThemes = _dbContext.MissionTheme
                .Where(mt => mt.Status == "active")
                .Select(mt => new DropDownResponseModel(mt.Id, mt.ThemeName))
                .Distinct()
                .ToList();

            return missionThemes;
        }

        public async Task<bool> UpdateMission(MissionRequestViewModel mission)
        {
            var missionInDb = await _dbContext.Missions.FindAsync(mission.Id);

            if (missionInDb == null || missionInDb.IsDeleted)
                throw new Exception("Mission not found");

            missionInDb.MissionTitle = mission.MissionTitle;
            missionInDb.MissionDescription = mission.MissionDescription;
            missionInDb.CityId = mission.CityId;
            missionInDb.CountryId = mission.CountryId;
            missionInDb.MissionSkillId = mission.MissionSkillId.ToString();
            missionInDb.MissionThemeId = mission.MissionThemeId;
            missionInDb.MissionOrganisationName = mission.MissionOrganisationName;
            missionInDb.MissionOrganisationDetail = mission.MissionOrganisationDetail;
            missionInDb.MissionType = mission.MissionType;
            missionInDb.TotalSheets = mission.TotalSeats;
            missionInDb.StartDate = DateTime.SpecifyKind(mission.StartDate, DateTimeKind.Utc);
            missionInDb.EndDate = DateTime.SpecifyKind(mission.EndDate, DateTimeKind.Utc);

            if (!string.IsNullOrWhiteSpace(mission.MissionImages))
            {
                if (string.IsNullOrWhiteSpace(missionInDb.MissionImages))
                    missionInDb.MissionImages = mission.MissionImages;
                else
                    missionInDb.MissionImages = $"{missionInDb.MissionImages},{mission.MissionImages}";
            }

            missionInDb.ModifiedDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return true;
        }


        public List<DropDownResponseModel> MissionSkillList()
        {
            var missionSkill = _dbContext.MissionSkill
                .Where(ms => ms.Status == "active")
                .Select(ms => new DropDownResponseModel(ms.Id, ms.SkillName))
                .ToList();

            return missionSkill;
        }

        //public List<MissionResponseModel> ClientMissionList(int userId)
        //{
        //    var missions = _dbContext.Missions
        //                    .Where(x => !x.IsDeleted)
        //                    .OrderBy(x => x.StartDate)
        //                    .Select(x => new MissionResponseModel()
        //                    {
        //                        Id = x.Id,
        //                        CityId = x.CityId,
        //                        CityName = x.City.CityName,
        //                        CountryId = x.CountryId,
        //                        CountryName = x.Country.CountryName,
        //                        EndDate = x.EndDate,
        //                        MissionDescription = x.MissionDescription,
        //                        MissionImages = x.MissionImages,
        //                        MissionOrganisationDetail = x.MissionOrganisationDetail,
        //                        MissionOrganisationName = x.MissionOrganisationName,
        //                        MissionSkillId = x.MissionSkillId,
        //                        MissionThemeId = x.MissionThemeId,
        //                        MissionThemeName = x.MissionTheme.ThemeName,
        //                        MissionTitle = x.MissionTitle,
        //                        MissionType = x.MissionType,
        //                        StartDate = x.StartDate,
        //                        TotalSheets = x.TotalSheets
        //                    }).ToList();
        //    return missions;
        //}

        public List<MissionResponseModel> ClientMissionList(int userId)
        {
            var missions = _dbContext.Missions
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.StartDate)
                .Select(x => new MissionResponseModel
                {
                    Id = x.Id,
                    CityId = x.CityId,
                    CityName = x.City.CityName,
                    CountryId = x.CountryId,
                    CountryName = x.Country.CountryName,
                    EndDate = x.EndDate,
                    MissionDescription = x.MissionDescription,
                    MissionImages = x.MissionImages,
                    MissionOrganisationDetail = x.MissionOrganisationDetail,
                    MissionOrganisationName = x.MissionOrganisationName,
                    MissionSkillId = x.MissionSkillId,
                    MissionThemeId = x.MissionThemeId,
                    MissionThemeName = x.MissionTheme.ThemeName,
                    MissionTitle = x.MissionTitle,
                    MissionType = x.MissionType,
                    StartDate = x.StartDate,
                    TotalSheets = x.TotalSheets,

                    // New fields:
                    IsApplied = _dbContext.MissionApplications
                        .Any(ma => ma.MissionId == x.Id && ma.UserId == userId && !ma.IsDeleted),

                    ApplicationStatus = _dbContext.MissionApplications
                        .Where(ma => ma.MissionId == x.Id && ma.UserId == userId && !ma.IsDeleted)
                        .Select(ma => ma.Status ? "Approved" : "Pending")
                        .FirstOrDefault()
                })
                .ToList();

            return missions;
        }


        public List<MissionResponseModel> MissionList()
        {
            var missions = _dbContext.Missions.Where(x => !x.IsDeleted)
                .Select(x => new MissionResponseModel()
                {
                    Id = x.Id,
                    CityId = x.CityId,
                    CityName = x.City.CityName,
                    CountryId = x.CountryId,
                    CountryName = x.Country.CountryName,
                    EndDate = x.EndDate,
                    MissionDescription = x.MissionDescription,
                    MissionImages = x.MissionImages,
                    MissionOrganisationDetail = x.MissionOrganisationDetail,
                    MissionOrganisationName = x.MissionOrganisationName,
                    MissionSkillId = x.MissionSkillId,
                    MissionThemeId = x.MissionThemeId,
                    MissionThemeName = x.MissionTheme.ThemeName,
                    MissionTitle = x.MissionTitle,
                    MissionType = x.MissionType,
                    StartDate = x.StartDate,
                    TotalSheets = x.TotalSheets
                }).ToList();
            return missions;
        }

        public string AddMission(AddMissionRequestModel request)
        {

            var exists = _dbContext.Missions.Any(x => x.MissionTitle.ToLower() == request.MissionTitle.ToLower()
                                                        && x.CityId == request.CityId
                                                        && x.StartDate.Date == request.StartDate.Date
                                                        && x.EndDate.Date == request.EndDate.Date && !x.IsDeleted);
            if (exists)
            {
                throw new Exception("Mission already exist");
            }

            var mission = new Missions()
            {
                MissionDescription = request.MissionDescription,
                MissionImages = request.MissionImages,
                CityId = request.CityId,
                CountryId = request.CountryId,
                MissionOrganisationDetail = request.MissionOrganisationDetail,
                MissionOrganisationName = request.MissionOrganisationName,
                MissionSkillId = request.MissionSkillId,
                MissionThemeId = request.MissionThemeId,
                MissionTitle = request.MissionTitle,
                MissionType = request.MissionType,
                StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc),
                CreatedDate = DateTime.UtcNow,
                TotalSheets = request.TotalSheets,
                IsDeleted = false
            };

            _dbContext.Missions.Add(mission);
            _dbContext.SaveChanges();
            return "Mission Save Successfully";
        }

        public MissionResponseModel GetMissionById(int missionId)
        {
            return _dbContext.Missions
                .Where(x => x.Id == missionId && !x.IsDeleted)
                .Select(x => new MissionResponseModel
                {
                    Id = x.Id,
                    CityId = x.CityId,
                    CityName = x.City.CityName,
                    CountryId = x.CountryId,
                    CountryName = x.Country.CountryName,
                    EndDate = x.EndDate,
                    MissionDescription = x.MissionDescription,
                    MissionImages = x.MissionImages,
                    MissionOrganisationDetail = x.MissionOrganisationDetail,
                    MissionOrganisationName = x.MissionOrganisationName,
                    MissionSkillId = x.MissionSkillId,
                    MissionThemeId = x.MissionThemeId,
                    MissionThemeName = x.MissionTheme.ThemeName,
                    MissionTitle = x.MissionTitle,
                    MissionType = x.MissionType,
                    StartDate = x.StartDate,
                    TotalSheets = x.TotalSheets
                }).FirstOrDefault() ?? throw new Exception("Mission not found");
        }

        public string DeleteMissionById(int missionId)
        {
            var mission = _dbContext.Missions.Where(x => x.Id == missionId && !x.IsDeleted).ExecuteUpdate(x => x.SetProperty(p => p.IsDeleted, true));
            return "Mission deleted successfully";
        }

        public string ApplyMission(ApplyMissionRequestModel request)
        {

            var mission = _dbContext.Missions.Where(x => x.Id == request.MissionId && !x.IsDeleted).FirstOrDefault();

            if (mission == null) { throw new Exception("Mission Not Found"); }

            if (mission.TotalSheets == 0) { throw new Exception("Mission housefull"); }

            if (mission.TotalSheets < request.Sheets) { throw new Exception("Not available seats"); }

            var missionApplication = new MissionApplication()
            {
                MissionId = request.MissionId,
                UserId = request.UserId,
                //AppliedDate = request.AppliedDate,
                 AppliedDate = DateTime.SpecifyKind(request.AppliedDate, DateTimeKind.Utc),
                Status = request.Status,
                Sheet = request.Sheets,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            _dbContext.MissionApplications.Add(missionApplication);
            _dbContext.SaveChanges();

            mission.TotalSheets -= request.Sheets;

            //_dbContext.Missions.Update(mission);
            _dbContext.SaveChanges();

            return "Mission applied successfully";

        }

        public string ApproveMission(int id)
        {
            var exists = _dbContext.MissionApplications.Any(x => x.Id == id);

            if (!exists) { throw new Exception("Mission application not exist"); }

            var updateCount = _dbContext.MissionApplications.Where(x => x.Id == id).ExecuteUpdate(m => m.SetProperty(property => property.Status, true));

            return updateCount > 0 ? "Mission approved" : "Mission is not approved";
        }


        public List<MissionApplicationResponseModel> MissionApplicationList()
        {
            return _dbContext.MissionApplications
                .Where(m => !m.IsDeleted && !m.Mission.IsDeleted && !m.User.IsDeleted)
                .Select(m => new MissionApplicationResponseModel()
                {
                    Id = m.Id,
                    AppliedDate = m.AppliedDate,
                    MissionId = m.MissionId,
                    MissionTheme = m.Mission.MissionTheme.ThemeName,
                    MissionTitle = m.Mission.MissionTitle,
                    Sheets = m.Sheet,
                    Status = m.Status,
                    UserId = m.UserId,
                    UserName = $"{m.User.FirstName} {m.User.LastName}",
                }).ToList();
        }

        public string DeleteMissionApplication(int id)
        {
            var mission = _dbContext.MissionApplications.Where(x => x.Id == id && !x.IsDeleted).ExecuteUpdate(x => x.SetProperty(p => p.IsDeleted, true));
            return "Mission application deleted successfully";
        }

    }
}
