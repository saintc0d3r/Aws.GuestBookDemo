using System;
using System.Collections.Generic;
using System.Linq;

using Amazon.SimpleDB.Model;
using GuestBook.Domain.Models;
using Xtremecode.Infrstructure.Aws.SimpleDb;

namespace GuestBook.Repository.Aws.SimpleDb
{
    class GuestBookEntryRepository : SimpleDbRepositoryBase<GuestBookEntry, long>, IGuestBookEntryRepository
    {
        const string DOMAIN_NAME = "GuestBookDemo";
        private const string QUERY_FIND_BY_NAME = "select * from {0} where `GuestName` = '{1}'";
        private const string QUERY_FIND_ALL_TOP = "select * from {0} where itemName() > '0' order by itemName() desc limit {1}";

        public GuestBookEntryRepository() : base(DOMAIN_NAME) { }

        public GuestBookEntry FindByName(string guestName)
        {
            var response = SimpleDb.Select(new SelectRequest
                            {
                                SelectExpression = string.Format(QUERY_FIND_BY_NAME, DOMAIN_NAME, guestName)
                            });

            // TODO: Look ugly. Need to refactor this.
            return (from item in response.Items
                    where item.Attributes.Any(attribute => attribute.Name.Equals("GuestName") &&
                          !string.IsNullOrWhiteSpace(attribute.Value) &&
                          attribute.Value.Equals(guestName))
                    select new GuestBookEntry
                        {
                            Id = long.Parse(item.Name),
                            GuestName = item.Attributes.GetValue("GuestName"),
                            Comment = item.Attributes.GetValue("Comment"),
                            PhotoUrl = item.Attributes.GetValue("PhotoUrl"),
                            CreateDate = DateTime.Parse(item.Attributes.GetValue("CreateDate")),
                            ModifiedDate = DateTime.Parse(item.Attributes.GetValue("ModifiedDate")),
                            ModifiedBy = uint.Parse(item.Attributes.GetValue("ModifiedBy"))
                        }).FirstOrDefault();
        }

        public IEnumerable<GuestBookEntry> FindLatestRecords(int top)
        {
            var response = SimpleDb.Select(new SelectRequest
                            {
                                SelectExpression = string.Format(QUERY_FIND_ALL_TOP, DOMAIN_NAME, top)
                            });
            return response.Items.Select(item => new GuestBookEntry
                {
                    Id = long.Parse(item.Name),
                    GuestName = item.Attributes.GetValue("GuestName"),
                    Comment = item.Attributes.GetValue("Comment"),
                    PhotoUrl = item.Attributes.GetValue("PhotoUrl"),
                    CreateDate = DateTime.Parse(item.Attributes.GetValue("CreateDate")),
                    ModifiedDate = DateTime.Parse(item.Attributes.GetValue("ModifiedDate")),
                    ModifiedBy = uint.Parse(item.Attributes.GetValue("ModifiedBy"))
                }).OrderByDescending(guestBookEntry => guestBookEntry.CreateDate).ToArray();
        }

    }
}
