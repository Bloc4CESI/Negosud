import Header from "../../../../../modules/layout/header";
import Footer from "../../../../../modules/layout/footer";
import OrderDetails from "../../../../../modules/account/orders/ordersDetails";

export default function Page({ params }: { params: { id: number }}) {
  const id = params;



  return (
    <>
    <Header/>
      <OrderDetails />
    <Footer/>
    </>
  )
}