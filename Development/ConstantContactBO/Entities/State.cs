using System;

namespace ConstantContactBO.Entities
{
    /// <summary>
    /// State Entity
    /// </summary>
    [Serializable]
    public class State
    {
        #region Members
        private string _code;
        private string _name;
        #endregion

        #region Properties
        /// <summary>
        /// Name of the State
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        /// <summary>
        /// Code of the State
        /// </summary>
        public string Code
        {
            get { return this._code; }
            set { this._code = value; }
        }
        #endregion
    }
}