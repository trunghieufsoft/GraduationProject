using Models.Common;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;

namespace Models.DataAccess
{
    public class UserDao
    {
        #region Singleton
        /**
         * Constants
         */
        private ShopDbContext db = null;
        private const string paramOne = "@uname";
        private const string paramTwo = "@passwd";
        private const string MP_UserLogin = "MP_UserLogin " + paramOne + ", " + paramTwo;

        /**
         * @description -- init
         */

        private UserDao()
        {
            db = new ShopDbContext();
        }

        private static UserDao instance = null;

        public static UserDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserDao();
                }
                return instance;
            }
        }
        #endregion Singleton
        
        #region Handle
        /**
         * @description -- get User by UserID
         * @param _key: string -- is field UserID
         */

        public User getByID(string _key, string include = null)
        {
            return include != null ? db.Users.Include(include).SingleOrDefault(x => x.UserID == _key) : db.Users.SingleOrDefault(x => x.UserID == _key);
        }

        /**
         * @description -- check exits user in table User with phone or mail(if exits)
         * @param _cate: Category -- is a transion object
         * @return -- fale when data response is had exits a object user.Phone or user.Email
         */

        public bool hasUser(User _request)
        {
            var user = (_request.Email != null && _request.Email != Constants.stringEmpty) ?
                db.Users.SingleOrDefault(obj => obj.Phone == _request.Phone || obj.Email == _request.Email)
                :
                db.Users.SingleOrDefault(obj => obj.Phone == _request.Phone);
            return user != default(User) ? true : false;
        }

        /**
         * @description -- insert a account
         * @param _request: User -- entity object
         */

        public bool insert(User _request)
        {
            try
            {
                if (!hasUser(_request))
                {
                    _request.CreatedAt = DateTime.Now;
                    _request.Password = Encrypt.Encrypt_Code(_request.Password, true);
                    db.Users.Add(_request);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }

        /**
         * @description -- delete a account
         * @param _key: string -- is field UserID
         */

        public bool delete(string _key)
        {
            if (hasReference(_key))
                return false;
            db.Users.Remove(getByID(_key));
            db.SaveChanges();
            return true;
        }

        /**
         * @description -- change status active
         * @param _key: string -- is field UserID
         */

        public bool changeStatus(string _key)
        {
            var user = getByID(_key);
            user.Phone = user.Phone == "" ? "03" : user.Phone;
            user.isActive = !user.isActive;
            user.UpdatedAt = DateTime.Now;
            try
            {
                db.SaveChanges();
                return user.isActive;
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /**
         * @description -- change new password
         * @param _request: UserRequestDto -- is the data transmitted down from the display screen
         */

        public bool changePasswd(UserRequestDto _request)
        {
            var user = getByID(_request.UserID);
            var new_password = Encrypt.Encrypt_Code(_request.Password, true);
            if (user.Password.Equals(new_password))
                return false;
            user.Password = new_password;
            user.UpdatedAt = DateTime.Now;
            db.SaveChanges();
            return true;
        }

        /**
         * @description -- update info account
         * @param _request: UserRequestDto -- is the data transmitted down from the display screen
         */

        public bool update(UserRequestDto _request)
        {
            var user = getByID(_request.UserID);
            user.FullName = _request.FullName;
            user.Address = _request.Address;
            user.Phone = _request.Phone;
            user.Email = _request.Email;
            if (!string.IsNullOrEmpty(_request.Password))
            {
                var new_password = Encrypt.Encrypt_Code(_request.Password, true);
                if (!user.Password.Equals(new_password))
                    user.Password = new_password;
            }
            user.UpdatedAt = DateTime.Now;
            db.SaveChanges();
            return true;
        }

        /**
         * @description -- get account list by search key
         * @param _search: string -- is search key
         */

        public IEnumerable<User> getObjectList(string _search, string include = null)
        {
            var model = include != null ? db.Users.Include(include).ToList() : db.Users.ToList();
            if (_search != null)
            {
                model = model.Where(obj =>
                obj.UserID.Contains(_search) ||
                obj.FullName.Contains(_search)
                ).ToList();
            }

            return model;
        }

        public IEnumerable<User> getObjectList(string _search, int page, out int totalRows, out int totalPages, string include = null)
        {
            var model = include != null ? db.Users.Include(include).ToList() : db.Users.ToList();
            if (_search != null)
            {
                model = model.Where(obj =>
                obj.UserID.Contains(_search) ||
                obj.FullName.Contains(_search)
                ).ToList();
            }

            totalRows = model.Count();
            totalPages = (int)Math.Ceiling((double)totalRows / Constants.PageSize);

            return model.OrderBy(u => u.CreatedAt)
                        .Skip((page - 1) * Constants.PageSize)
                        .Take(Constants.PageSize);
        }

        /**
         * @description -- check login success
         * @param _user: UserDto -- data transion object
         * @param _isSP: boolean -- default = false, allowed to use store
         */

        public Constants.LoginState checkLogin(UserDto _user, bool _isSP = false)
        {
            // check validator
            if (string.IsNullOrEmpty(_user.UserName) && string.IsNullOrEmpty(_user.Password))
                return Constants.LoginState.UsernameAndPasswordNull;
            if (string.IsNullOrEmpty(_user.UserName))
                return Constants.LoginState.UsernameNull;
            if (string.IsNullOrEmpty(_user.Password))
                return Constants.LoginState.PasswordNull;
            // encode
            string encrypt = Encrypt.Encrypt_Code(_user.Password, true);
            // create parameter to use store
            object[] parameter =
            {
                new SqlParameter(paramOne, _user.UserName),
                new SqlParameter(paramTwo,  encrypt)
            };
            var resQuery = _isSP ?
                db.Database.SqlQuery<bool>(MP_UserLogin, parameter).SingleOrDefault()
                :
               hasUser(_user.UserName, encrypt);
            //hasUser(_user.username, _user.password);
            // return zero when login failed, account exists if locked returns two number else returns one number
            return !resQuery ? Constants.LoginState.Failed : getByID(_user.UserName).isActive ? Constants.LoginState.Successed : Constants.LoginState.Failed;
        }

        /**
         * @private
         * @description -- check the existence of accounts
         * @param _uname: string -- is field UserID
         * @param _pass: string -- is field Password
         */

        private bool hasUser(string _uname, string _pass)
        {
            var User = db.Users.SingleOrDefault(x => x.UserID == _uname && x.Password == _pass && x.Grant.isActive == true);
            return User != default(User) ? true : false;
        }

        public bool checkUserName(string username)
        {
            return db.Users.Count(x => x.UserID == username) > 0;
        }

        private bool hasReference(string _key)
        {
            var account = getByID(_key);
            if (account != default(User))
            {
                var count_one = db.Bills.Where(obj => obj.UserID == _key).ToList().Count;
                var count_two = db.Replies.Where(obj => obj.UserID == _key).ToList().Count;
                var count_three = db.Comments.Where(obj => obj.UserID == _key).ToList().Count;
                return (count_one + count_two + count_three) > 0;
            }
            return false;
        }
        #endregion Handle
    }
}