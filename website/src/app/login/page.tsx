import Login from "../../../modules/auth/login";
import Header from "../../../modules/layout/header";
import Footer from "../../../modules/layout/footer";
import '../globals.css';

export default function LoginPage()  {
  return (
<>
  <Header/>
  <Login/>
  <Footer/>
</>
  )
};
