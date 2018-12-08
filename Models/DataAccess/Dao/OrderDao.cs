using Models.Common;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DataAccess
{
    public class OrderDao
    {
        #region Singleton
        /**
         * Constants 
         */
        private ShopDbContext db = null;

        /**
         * @description -- init
         */
        public OrderDao()
        {
            db = new ShopDbContext();
        }
        public static OrderDao instance;

        public static OrderDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderDao();
                }
                return instance;
            }
        }

        #endregion Singleton

        #region Handle
        /**
         * @description -- get Order by OrderID
         * @param _keyBill: int -- is field BillID
         * @param _keyPro: int -- is field ProID
         */
        public Order getByID(string _keyBill, int _keyPro)
        {
            return db.Orders.SingleOrDefault(obj => obj.ProdID == _keyPro && obj.BillID == _keyBill);
        }

        /**
         * @description -- insert a Order
         * @param _request: Order -- entity object
         */
        public bool insert(Order _request)
        {
            db.Orders.Add(_request);
            db.SaveChanges();
            BillDao.Instance.changeBillTotalPrice(_request.BillID, totalPrice(_request.BillID));
            return Constants.trueValue;
        }

        /**
         * @description -- delete a Order
         * @param _keyBill: int -- is field BillID
         * @param _keyPro: int -- is field ProID
         */
        public bool delete(string _keyBill, int _keyPro)
        {
            db.Orders.Remove(getByID(_keyBill, _keyPro));
            db.SaveChanges();
            BillDao.Instance.changeBillTotalPrice(_keyBill, totalPrice(_keyBill));
            return Constants.trueValue;
        }

        /**
         * @description -- change new order count
         * @param _request: OrderRequestDto -- is the data transmitted down from the display screen
         */
        public bool changeOrderCount(OrderRequestDto _request)
        {
            var Order = getByID(_request.BillID, _request.ProdID);
            Order.Count = _request.Count;
            Order.UpdatedAt = DateTime.Now;
            db.SaveChanges();
            BillDao.Instance.changeBillTotalPrice(_request.BillID, totalPrice(_request.BillID));
            return Constants.trueValue;
        }

        /**
         * @description -- get Order list by search key (BillID)
         * @param _keyBill: int -- is search key
         */
        public IEnumerable<Order> getObjectList(string _keyBill)
        {
            return db.Orders.Where(obj => obj.BillID.ToString().Contains(_keyBill.ToString()));
        }

        /**
         * private
         * @description -- Calculate the total price of the product
         * @param _keyBill: int -- is search key
         */
        private int totalPrice(string _keyBill)
        {
            var orderList = getObjectList(_keyBill).ToList();
            if (orderList.Count > 0)
            {
                int total = Constants.zeroNumber;
                foreach (var item in orderList)
                {
                    total += item.Count * item.Product.Cost;
                }
                return total;
            }
            return Constants.zeroNumber;
        }
        #endregion Handle
    }
}
