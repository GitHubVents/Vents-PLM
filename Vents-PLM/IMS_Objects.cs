using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vents_PLM
{
    class IMS_Object
    {

        String m_OBJECT_ID;
        [Browsable(true)]
        [Description("")]
        [Category("Идентификация")]
        [DisplayName("OBJECT_ID")]
        [ReadOnly (true)]
        public String OBJECT_ID
        {
            get { return m_OBJECT_ID; }
        }

        String m_OBJECT_TYPE;
        [Browsable(true)]
        [Description("")]
        [Category("Идентификация")]
        [DisplayName("OBJECT_TYPE")]
        public String OBJECT_TYPE
        {
            get { return m_OBJECT_TYPE; }
            set { m_OBJECT_TYPE = value; }
        }

        String m_OBJ_CREATE;
        [Browsable(true)]
        [Description("")]
        [Category("Идентификация")]
        [DisplayName("OBJ_CREATE")]
        public String OBJ_CREATE
        {
            get { return m_OBJ_CREATE; }
            set { m_OBJ_CREATE = value; }
        }
    }
}
