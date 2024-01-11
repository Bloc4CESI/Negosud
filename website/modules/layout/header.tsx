import Image from 'next/image'
import img from "../../src/app/images/logo.png"
import Link from 'next/link';

const Header = () => {

  return (
    <header className="sticky top-0 z-[20] mx-auto flew w-full items-center justify-between p-4 flex bg-black text-sm text-white">
      <Link href={'/home'} >
      <Image src={img} width={80} height={80} alt={""}/>
      </Link>
      <Link href={'/about'} >
      <button>À propos</button>
      </Link>
      <Link href={'/login'} >
      <button>Connectez-vous</button>
      </Link>
    </header>
  );
};

export default Header;
