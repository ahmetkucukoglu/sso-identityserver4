namespace SSO.Application.Infrastructure.AspNet
{
    using Domain.Entities;
    using Domain.EntityFramework;
    using Microsoft.AspNetCore.DataProtection.Repositories;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Xml.Linq;

    public class DataProtectionKeyRepository : IXmlRepository
    {
        private readonly AuthDbContext _dbContext;

        public DataProtectionKeyRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            return new ReadOnlyCollection<XElement>(_dbContext.DataProtectionKeys.Select((x) => XElement.Parse(x.XmlData)).ToList());
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            var entity = _dbContext.DataProtectionKeys.FirstOrDefault(k => k.FriendlyName == friendlyName);

            if (null != entity)
            {
                entity.XmlData = element.ToString();

                _dbContext.DataProtectionKeys.Update(entity);
            }
            else
            {
                _dbContext.DataProtectionKeys.Add(new DataProtectionKey
                {
                    FriendlyName = friendlyName,
                    XmlData = element.ToString()
                });
            }

            _dbContext.SaveChanges();
        }
    }
}
