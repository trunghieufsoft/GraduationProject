using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Models.Common.Encode
{
    public static class Converter
    {
        private const string comma = ",";
        public enum ItemTypes
        {
            Bill,
            Comment,
            Rating,
            Reply
        };

        public static Image byteArrayToImage(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }

        public static byte[] imageToByteArray(string imagefilePath)
        {
            Image image = Image.FromFile(imagefilePath);
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        /**
         * @private
         * @description -- Automatic code generation
         * @param imagefilePath: string -- is the path of the image file
         */

        public static string genCode(string _name)
        {
            for (int i = 33; i < 48; i++)
            {
                _name = _name.Replace(((char)i).ToString(), "");
            }

            for (int i = 58; i < 65; i++)
            {
                _name = _name.Replace(((char)i).ToString(), "");
            }

            for (int i = 91; i < 97; i++)
            {
                _name = _name.Replace(((char)i).ToString(), "");
            }
            for (int i = 123; i < 127; i++)
            {
                _name = _name.Replace(((char)i).ToString(), "");
            }
            _name = _name.Replace(" ", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = _name.Normalize(System.Text.NormalizationForm.FormD);
            return stringToLower(regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D'));
        }

        public static string stringToLower(string _str)
        {
            var charArray = _str.ToCharArray(0, _str.Length);
            var result = "";
            foreach (var character in charArray)
            {
                result += Char.IsLower(character) ? character : Char.ToLower(character);
            }
            return result;
        }

        public static string stringToUpper(string _str)
        {
            var charArray = _str.ToCharArray(0, _str.Length);
            var result = "";
            foreach (var character in charArray)
            {
                result += Char.IsUpper(character) ? character : Char.ToUpper(character);
            }
            return result;
        }

        // method create id format type <<ddmmyy[\d]>> : using with regex
        public static string genIdFormat_ddmmyy(ShopDbContext db, ItemTypes table)
        {
            //table = table.ToLower();
            string now = DateTime.Now.ToString("ddMMyy");
            var indexString = "";
            var indexNumber = 0;
            List<string> listData = null;
            switch (table)
            {
                case ItemTypes.Bill:
                    listData = db.Bills
                            .Where(obj => obj.BillID.Contains(now))
                            .Select(s => s.BillID).ToList();
                    break;
                case ItemTypes.Comment:
                    listData = db.Comments
                            .Where(obj => obj.ComID.Contains(now))
                            .Select(s => s.ComID).ToList();
                    break;
                case ItemTypes.Rating:
                    listData = db.Ratings
                            .Where(obj => obj.RatID.Contains(now))
                            .Select(s => s.RatID).ToList();
                    break;
                default:
                    return "NotTable";
            }
            if (listData != null)
            {
                var count = 0;
                foreach (var item in listData)
                {
                    indexString = item.ToString().Substring(6);
                    indexNumber = Convert.ToInt32(indexString);
                    if (count != indexNumber)
                    {
                        break;
                    }
                    count++;
                    indexNumber++;
                }
            }
            indexString = indexNumber.ToString();
            // format type ddmmyyxxxx
            now += addZeroString(indexString);
            return now;
        }

        // add zero number into the end of string
        private static string addZeroString(string indexString)
        {
            return indexString.Length == 4 ? indexString : addZeroString("0" + indexString);
        }

        // method create id format for Reply
        public static int genIdFormat_numberNo(ShopDbContext db, string _comID, ItemTypes table = ItemTypes.Reply)
        {
            List<int> listData = null;
            switch (table)
            {
                case ItemTypes.Reply:
                    listData = db.Replies
                            .Where(obj => obj.ComID.Contains(_comID))
                            .Select(s => s.RepNo).ToList();
                    break;
                case ItemTypes.Bill:
                // TODO
                case ItemTypes.Comment:
                // TODO
                case ItemTypes.Rating:
                // TODO
                default:
                    return -1; // error: return false
            }

            return listData.Count > 0 ? listData[listData.Count - 1] + 1 : 1;
        }

        public static string formatPrice(int cost)
        {
            string priceString = cost.ToString();
            Regex rgx = new Regex(@"(\d+)(\d{3})");
            var res = rgx.IsMatch(priceString);
            while (res)
            {
                priceString = rgx.Replace(priceString, "$1" + comma + "$2");
                res = rgx.IsMatch(priceString);
            }
            return priceString;
        }
    }
}