namespace EFCoreRelationshipsTutorial
{
    public class AddCharacterSkillDTO
    {
        public int CharacterId { get; set; }
        public int SkillId { get; set; }
    }

    public class AddCharacterSkillMultipleDTO
    {
        public int CharacterId { get; set; }
        public List<int> SkillIds { get; set; }
    }
}
