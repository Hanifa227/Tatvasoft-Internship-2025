using Mission.Entities.Models.CommonModel;
using Mission.Entities.Models.MissionsModels;

namespace Mission.Repositories.IRepositories
{
    public interface IMissionRepository
    {
        List<MissionResponseModel> MissionList();
        string AddMission(AddMissionRequestModel request);
        MissionResponseModel GetMissionById(int missionId);
        string DeleteMissionById(int missionId);
        Task<bool> UpdateMission(MissionRequestViewModel model);
        List<MissionResponseModel> ClientMissionList(int userId);

        List<DropDownResponseModel> MissionThemeList();

        List<DropDownResponseModel> MissionSkillList();
        string ApplyMission(ApplyMissionRequestModel request);
        string ApproveMission(int id);
        string DeleteMissionApplication(int id);
        List<MissionApplicationResponseModel> MissionApplicationList();
    }
}
