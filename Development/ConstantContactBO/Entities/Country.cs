using System;

namespace ConstantContactBO.Entities
{
    /// <summary>
    /// Country Entity
    /// </summary>
    [Serializable]
    public class Country
    {
        #region Members
        private string _code;
        private string _name;
        #endregion

        #region Properties
        /// <summary>
        /// Name of the Country
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        /// <summary>
        /// Code of the Country
        /// </summary>
        public string Code
        {
            get { return this._code; }
            set { this._code = value; }
        }
        #endregion
    }
}