import Header from "../../../modules/layout/header";
import RegisterForm from "../../../modules/auth/register";
import Footer from "../../../modules/layout/footer";
import "../globals.css";

export default function Register() {
    return (
        <>
          <Header/>
          <RegisterForm/>
          <Footer/>
        </>
    )
}