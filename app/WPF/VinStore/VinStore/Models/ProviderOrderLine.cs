using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace ApiNegosud.Models
{
    public class ProviderOrderLine : INotifyPropertyChanged
    {
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public Decimal Price { get; set; }

        [ForeignKey("ProviderOrder")]
        public int ProviderOrderId { get; set; }
        public virtual ProviderOrder? ProviderOrder { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public Decimal TotalPrice
        {
            get { return Quantity * Price; }
            set {
                OnPropertyChanged(nameof(TotalPrice));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
