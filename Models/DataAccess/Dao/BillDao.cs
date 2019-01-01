using Models.Common;
using Models.Common.Encode;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DataAccess
{
    public class BillDao
    {

        #region Singleton
        /**
         * Constants
         */
        private ShopDbContext db = null;

        /**
         * @description -- init
         */

        private BillDao()
        {
            db = new ShopDbContext();
        }

        private static BillDao instance = null;

        public static BillDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BillDao();
                }
                return instance;
            }
        }
        #endregion

        #region Handle
        /**
         * @description -- get bill by BillID
         * @param _key: string -- is field BillID
         */
        public Bill getByID(string _key)
        {
            return db.Bills.SingleOrDefault(x => x.BillID == _key);
        }

        public List<Product> BillDetails(string billId)
        {
            return db.Orders.Include("Products").Where(x => x.BillID == billId).Select(x => x.Product).ToList();
        }

        public List<int> ProductCountDetails(string billId)
        {
            return db.Orders.Include("Products").Where(x => x.BillID == billId).Select(x => x.Count).ToList();
        }

        /**
         * @description -- insert a bill
         * @param _request: Bill -- entity object
         */

        public string insert(Bill _request)
        {
            _request.Status = false;
            _request.CreatedAt = DateTime.Now;
            _request.BillID = Converter.genIdFormat_ddmmyy(db, Converter.ItemTypes.Bill);
            if (_request.UserID.Equals(""))
                _request.UserID = null;
            db.Bills.Add(_request);
            var x = db.Bills.ToList();
            db.SaveChanges();
            return _request.BillID;
        }

        public bool ChangeStatus(string billId)
        {
            try
            {
                var bill = db.Bills.Single(b => b.BillID == billId);
                bill.Status = !bill.Status;
                db.SaveChanges();
            }
            catch (Exception )
            {
                return false;
            }
            return true;
        }

        /**
         * @description -- delete a bill
         * @param _key: byte -- is field BillID
         */

        public bool delete(string _key)
        {
            // delete reference from table order
            var clear = removeReference(_key);
            var delItem = getByID(_key);
            if (clear && delItem != default(Bill))
            {
                db.Bills.Remove(delItem);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        /**
         * @description -- change new bill total price
         * @param _request: BillRequestDto -- is the data transmitted down from the display screen
         */

        public bool changeBillTotalPrice(string _billID, int totalPrice)
        {
            var bill = getByID(_billID);
            bill.TotalPrice = totalPrice;
            bill.UpdatedAt = DateTime.Now;
            db.SaveChanges();
            return true;
        }

        /**
         * @description -- change new bill note
         * @param _request: BillRequestDto -- is the data transmitted down from the display screen
         */

        public bool changeBillNote(BillRequestDto _request)
        {
            var bill = getByID(_request.BillID);
            bill.Note = _request.Note;
            bill.UpdatedAt = DateTime.Now;
            db.SaveChanges();
            return true;
        }

        /**
         * @description -- change new bill user
         * @param _request: BillRequestDto -- is the data transmitted down from the display screen
         */

        public bool changeBillUser(BillRequestDto _request)
        {
            var bill = getByID(_request.BillID);
            bill.UserID = _request.UserID;
            bill.UpdatedAt = DateTime.Now;
            db.SaveChanges();
            return true;
        }

        /**
         * @description -- update info bill
         * @param _request: BillRequestDto -- is the data transmitted down from the display screen
         */

        public bool update(BillRequestDto _request)
        {
            var bill = getByID(_request.BillID);
            bill.CustomerName = _request.CustomerName;
            bill.DeliveryAddress = _request.DeliveryAddress;
            bill.Phone = _request.Phone;
            bill.Note = _request.Note;
            bill.UpdatedAt = DateTime.Now;
            db.SaveChanges();
            return true;
        }

        /**
         * @description -- get Bill list by search key
         * @param _search: string -- is search key
         */

        public IEnumerable<Bill> getObjectList(int _search)
        {
            return db.Bills.Where(obj => obj.BillID.ToString().Contains(_search.ToString()));
        }


        public IEnumerable<Bill> GetBills(string userID, bool isStaff)
        {
            return isStaff ? db.Bills.ToList() : db.Bills.Where(b => b.UserID == userID).AsEnumerable();
        }

        /**
         * @description -- remove reference to table Bill by key
         * @param _key: string -- is field BillID
         */

        private bool removeReference(string _key)
        {
            if (hasReference(_key))
            {
                var reference = db.Orders.Where(obj => obj.BillID == _key).ToList();
                foreach (var item in reference)
                {
                    db.Orders.Remove(item);
                }
                db.SaveChanges();
            }
            return true;
        }

        /**
         * @description -- check has data reference to object
         * @param _key: int -- is field BillID
         */

        private bool hasReference(string _key)
        {
            var bill = getByID(_key);
            if (bill != default(Bill))
            {
                var count = db.Orders.Where(obj => obj.BillID == _key).ToList().Count;
                return count > 0 ? true : false;
            }
            return false;
        }

        #endregion Handle
    }
}