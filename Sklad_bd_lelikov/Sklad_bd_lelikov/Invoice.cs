//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sklad_bd_lelikov
{
    using System;
    using System.Collections.Generic;
    
    public partial class Invoice
    {
        public int ID_invoice { get; set; }
        public string Order_date { get; set; }
        public int ID_customer { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
