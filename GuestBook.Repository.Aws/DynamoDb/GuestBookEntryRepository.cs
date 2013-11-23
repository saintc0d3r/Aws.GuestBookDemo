using System;
using System.Collections;
using System.Collections.Generic;
using GuestBook.Domain.Models;

namespace GuestBook.Repository.Aws.DynamoDb
{
    class GuestBookEntryRepository : IGuestBookEntryRepository
    {
        public void Add(GuestBookEntry entity)
        {

            throw new NotImplementedException();
        }

        public IEnumerable<GuestBookEntry> Find(Func<GuestBookEntry, bool> criteria)
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<GuestBookEntry> entities)
        {
            throw new NotImplementedException();
        }

        public void Remove(GuestBookEntry guestBookEntry)
        {
            throw new NotImplementedException();
        }

        public GuestBookEntry FindByItemId(long itemId)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable entities)
        {
            throw new NotImplementedException();
        }
    }
}
