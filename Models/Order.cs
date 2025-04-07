
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Stateless;

namespace WebApiWithRoleAuthentication.Models
{
    public enum OrderStateEnum
    {
        Pending, // 訂單已生成
        Processing, // 支付處理中
        Completed, // 交易成功
        Declined, // 交易失败
        Cancelled, // 訂單取消
        Refund, // 已退款
    }
    public enum OrderStateTriggerEnum
    {
        PlaceOrder, // 支付
        Approve, // 收款成功
        Reject, // 收款失败
        Cancel, // 取消
        Return // 退货
    }

    public class Order
    {

        public Order()
        {
            StateMachineInit();
        }
        [Key]
        public Guid Id { get; set; }
        public string? UserId { get; set; }

        [JsonIgnore]
        public ApplicationUser? User { get; set; }

        [JsonIgnore]
        public ICollection<LineItem>? OrderItems { get; set; }
        public OrderStateEnum State { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public string? TransactionMetadata { get; set; }

        StateMachine<OrderStateEnum, OrderStateTriggerEnum>? _machine;


        public void PaymentProcessing()
        {
            _machine?.Fire(OrderStateTriggerEnum.PlaceOrder);
        }

        public void PaymentApprove()
        {
            _machine?.Fire(OrderStateTriggerEnum.Approve);
        }

        public void PaymentReject()
        {
            _machine?.Fire(OrderStateTriggerEnum.Reject);
        }
        private void StateMachineInit()
        {

            _machine = new StateMachine<OrderStateEnum, OrderStateTriggerEnum>(
         () => State,
          s => State = s
            );


            // Pending （待處理）
            //可通過 PlaceOrder（下訂單）轉到 Processing（處理中）
            //也可通過 Cancel轉到 Cancelled

            _machine.Configure(OrderStateEnum.Pending)
                .Permit(OrderStateTriggerEnum.PlaceOrder, OrderStateEnum.Processing)
                .Permit(OrderStateTriggerEnum.Cancel, OrderStateEnum.Cancelled);

            //Processing （處理中）
            //可通過 Approve轉到 Completed
            //或通過 Reject轉到 Declined

            _machine.Configure(OrderStateEnum.Processing)
                .Permit(OrderStateTriggerEnum.Approve, OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Reject, OrderStateEnum.Declined);

            //Declined 
            //可以通過觸發 PlaceOrder（下訂單）重新回到 Processing

            _machine.Configure(OrderStateEnum.Declined)
                .Permit(OrderStateTriggerEnum.PlaceOrder, OrderStateEnum.Processing);

            //Completed
            //可以通過 Return（退貨）轉到 Refund（退款）

            _machine.Configure(OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Return, OrderStateEnum.Refund);
        }
    }
}
