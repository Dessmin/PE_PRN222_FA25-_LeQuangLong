namespace SportsLendDB.BLL.DTOs
{
    public class CreateEquipmentDto
    {
        public string Name { get; set; }

        public string Brand { get; set; }

        public EquipmentCondition Condition { get; set; }

        public decimal DailyFeeUsd { get; set; }

        public int InStock { get; set; }

        public int Type { get; set; }
    }

    public enum EquipmentCondition
    {
        New,
        Good,
        Used,
        Damaged
    }
}
