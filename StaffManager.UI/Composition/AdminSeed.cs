namespace StaffManager.UI.Composition
{
    public sealed class AdminSeed
    {
        public int? StaffId { get; init; }
        public string? StaffName { get; init; }
        public bool IsStaffIdReadOnly { get; init; }
    }
}