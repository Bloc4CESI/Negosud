 "use client";
import React from "react";
import Cart from "../../public/img/cart.svg";
import User from "../../public/img/user.svg";

 const Header: React.FC = () => {
     return (
         <div>
              <h1>NEGOSUD</h1>
           <div>
            <Cart className="h-14 w-3"/>
            <User/>
           </div>
         </div>
     );
 };

export default Header;
