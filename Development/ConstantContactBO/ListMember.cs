using System;
using System.Collections.Generic;

using System.Text;

namespace ConstantContactBO
{
    public class ListMember
    {
        #region Fields
        private string _ID;
        /// <summary>
        /// ListMember's ID
        /// </summary>
        private string _Name;
        /// <summary>
        /// ListMember's Name
        /// </summary>
        private string _EmailAddress;
        /// <summary>
        /// ListMember's Name
        /// </summary>
        private string _Link;
        /// <summary>
        /// ListMember's Link
        /// </summary>
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the ListMember's ID
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        /// <summary>
        /// Gets or sets the ListMember's Name
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        /// <summary>
        /// Gets or sets the ListMember's Email Address
        /// </summary>
        public string EmailAddress
        {
            get { return _EmailAddress; }
            set { _EmailAddress = value; }
        }
        /// <summary>
        /// Gets or sets the ListMember's Link
        /// </summary>
        public string Link
        {
            get { return _Link; }
            set { _Link = value; }
        }
        #endregion
    }
}
