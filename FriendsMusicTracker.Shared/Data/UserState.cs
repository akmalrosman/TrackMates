namespace FriendsMusicTracker.Data
{
    public class UserState
    {
        public int MemberId { get; private set; }
        public string MemberName { get; private set; } = string.Empty;
        public bool IsLoggedIn => MemberId > 0;
        public event Action? OnChange;
        public void Login(int id, string name)
        {
            MemberId = id;
            MemberName = name;
            NotifyStateChanged();
        }
        public void Logout()
        {
            MemberId = 0;
            MemberName = string.Empty;
            NotifyStateChanged();
        }
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}