using User_Profile_Service.src._01_Domain.Core.Common;
using User_Profile_Service.src._01_Domain.Core.Enums;
using User_Profile_Service.src._01_Domain.Core.Events;
using User_Profile_Service.src._01_Domain.Core.ValueObjects;

namespace User_Profile_Service.src._01_Domain.Core.Aggregates.UserProfile
{
    public class UserProfile : AggregateRoot
    {
        public Guid UserId { get; private set; }
        public FullName Name { get; private set; }
        public UserContactInfo ContactInfo { get; private set; }
        public UserAvatar? Avatar { get; private set; }
        public BirthDate? BirthDate { get; private set; }
        public Gender Gender { get; private set; }
        public UserStatus Status { get; private set; }

        private readonly List<UserAddress> _addresses = new();
        public IReadOnlyCollection<UserAddress> Addresses => _addresses.AsReadOnly();

        public UserPreference Preferences { get; private set; }

        private UserProfile() { } // For EF Core

        public UserProfile(Guid userId, FullName name, UserContactInfo contactInfo, UserPreference preferences)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Name = name;
            ContactInfo = contactInfo;
            Preferences = preferences;
            Gender = Gender.Unknown;
            Status = UserStatus.Active;
        }

        public void UpdateName(FullName name)
        {
            Name = name;
            AddDomainEvent(new UserProfileUpdatedEvent(Id));
        }

        public void UpdateGender(Gender gender)
        {
            Gender = gender;
        }

        public void UpdateBirthDate(BirthDate birthDate)
        {
            BirthDate = birthDate;
        }

        public void UpdateContactInfo(UserContactInfo contactInfo)
        {
            ContactInfo = contactInfo;
        }

        public void UpdateAvatar(string avatarUrl)
        {
            if (Avatar == null)
                Avatar = new UserAvatar(avatarUrl);
            else
                Avatar.UpdateUrl(avatarUrl);

            AddDomainEvent(new UserAvatarChangedEvent(Id, avatarUrl));
        }

        public void AddAddress(UserAddress address)
        {
            if (address.IsDefault)
            {
                var existingDefault = _addresses.FirstOrDefault(a => a.Type == address.Type);
                if (existingDefault != null)
                    existingDefault.UnsetDefault();
            }

            _addresses.Add(address);
        }

        public void RemoveAddress(Guid addressId)
        {
            var address = _addresses.FirstOrDefault(a => a.Id == addressId);
            if (address != null)
                _addresses.Remove(address);
        }

        // متد جدید برای تنظیم آدرس پیش‌فرض
        public void SetAddressAsDefault(UserAddress address)
        {
            // اگر آدرس متعلق به این کاربر نیست
            if (!_addresses.Contains(address))
            {
                throw new InvalidOperationException("Address does not belong to this user profile.");
            }

            // غیرفعال کردن حالت پیش‌فرض برای بقیه آدرس‌های همین نوع
            foreach (var addr in _addresses.Where(a => a.Type == address.Type))
            {
                addr.UnsetDefault();
            }

            // فعال کردن حالت پیش‌فرض برای آدرس انتخابی
            address.SetAsDefault();
        }

        public void UpdatePreferences(UserPreference preferences)
        {
            Preferences = preferences;
            AddDomainEvent(new UserProfileUpdatedEvent(Id));
        }

        public void Deactivate()
        {
            Status = UserStatus.Inactive;
        }

        public void Suspend()
        {
            Status = UserStatus.Suspended;
        }
    }
}
