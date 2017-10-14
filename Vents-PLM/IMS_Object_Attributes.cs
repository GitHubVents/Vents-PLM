using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vents_PLM
{
   public class IMS_Object_Attributes
    {

        String m_ATTRIBUTE_ID;
        [Browsable(true)]
        [Description("")]
        [Category("Идентификация")]
        [DisplayName("OBJECT_TYPE")]
        public String ATTRIBUTE_ID
        {
            get { return m_ATTRIBUTE_ID; }
            set { m_ATTRIBUTE_ID = value; }
        }


        String m_OBJECT_ID;
        [Browsable(true)]
        [Description("")]
        [Category("Идентификация")]
        [DisplayName("OBJECT_ID")]
        public String OBJECT_ID
        {
            get { return m_OBJECT_ID; }
            set { m_OBJECT_ID = value; }
        }

        String m_INLIST_ID;
        [Browsable(true)]
        [Description("")]
        [Category("Идентификация")]
        [DisplayName("INLIST_ID")]
        public String INLIST_ID
        {
            get { return m_INLIST_ID; }
            set { m_INLIST_ID = value; }
        }

        String m_STRING_VALUE;
        [Browsable(true)]
        [Description("")]
        [Category("Идентификация")]
        [DisplayName("INLIST_ID")]
        public String STRING_VALUE
        {
            get { return m_STRING_VALUE; }
            set { m_STRING_VALUE = value; }
        }
    }
}
