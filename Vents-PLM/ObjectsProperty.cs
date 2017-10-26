using System;
using System.ComponentModel;

namespace Vents_PLM
{
   public class ObjectsProperty
    {
        String m_GUID;
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Уникальный глобальный идентификатор атрибута")]
        [Category("Идентификация")]
        [DisplayName("Глобальный идентификатор")]
        public String GUID
        {
            get { return m_GUID; }
            set { m_GUID = value; }
        }

        String m_NOTE;
        [Browsable(true)]
        [Description("Позволяет ввести комментарий для данного атрибута, поясняющий его назначение. Пользователь сможет увидеть данный комментарий в карточке объекта, выделив атрибут в списке атрибутов объекта или связи на закладке Свойства или Свойства связей.")]
        [Category("Идентификация")]
        [DisplayName("Комментарий")]
        public String NOTE
        {
            get { return m_NOTE; }
            set { m_NOTE = value; }
        }


        String m_SHORT_NAME;
        [Browsable(true)]
        [Description("Краткое наименование данного атрибута (до 32 символов). Может содержать пустые и неуникальные значения. Используется для отображения в таблицах с различной справочной информацией, упрощенного отображения формул в экспертной системе и т.д.")]
        [Category("Идентификация")]
        [DisplayName("Краткое наименование")]
        public String SHORT_NAME
        {
            get { return m_SHORT_NAME; }
            set { m_SHORT_NAME = value; }
        }


        String m_NAME;
        [Browsable(true)]
        [Description("Позволяет ввести уникальное наименование атрибута (до 255 символов)")]
        [Category("Идентификация")]
        [DisplayName("Наименование")]
        public String  NAME
        {
            get { return m_NAME; }
            set { m_NAME = value; }
        }

        String m_TYPE_NAME;
        [Browsable(true)]
        [Description("Поле, обязательное для заполнения. Содержит наименование объектов данного типа (например, “Деталь”). Может быть длиной до 255 символов.")]
        [Category("Идентификация")]
        [DisplayName("Имя объекта")]
        public String TYPE_NAME
        {
            get { return m_TYPE_NAME; }
            set { m_TYPE_NAME = value; }
        }

        String m_OBJECT_TYPE;
        [Browsable(true)]
        [Description("")]
        [Category("Идентификация")]
        [DisplayName("m_OBJECT_TYPE")]
        public String OBJECT_TYPE
        {
            get { return m_OBJECT_TYPE; }
            set { m_OBJECT_TYPE = value; }
        }


        public ObjectsProperty()
        {
            m_NAME = "Новый объект";
            GUID = Guid.NewGuid().ToString();
            //OBJECT_ID = "0";
        }

    }
}