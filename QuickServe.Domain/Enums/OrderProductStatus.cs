using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Domain.Enums
{
    public enum OrderItemStatus
    {
        Pending,    // Chờ xác nhận
        Confirmed,  // Đã xác nhận
        InProgress, // Đang chuẩn bị
        Ready,      // Sẵn sàng
        Delivered,  // Đã giao
        Cancelled   // Đã hủy
    }
}
