import Header from "../../modules/layout/header";
import "./globals.css";
import Footer from "../../modules/layout/footer";
import ArrowRight from "../../modules/svg/arrowRight.svg";
import Link from 'next/link';

export default function Home() {
  return (
    <>
      <div className="bg-[url(../../public/images/vin_accueil_bg.jpg)] bg-clip-border bg-cover bg-center h-[750px]">
        <Header style={"bg-transparent text-white stroke-white"}/>
        <Link href="./listingProducts" className="h-full w-full">
        <p className=" w-full h-[680px] bottom-0 flex items-end">
          <div className="flex items-end text-white text-xl font-bold mb-4 ml-6">
            Notre listing
            <ArrowRight className="ml-1 stroke-white"/>
          </div>
        </p>
        </Link>
      </div>
      
      <div className="md:container py-1 md:mx-auto">
        <div className="flex flex-col md:flex-row items-center bg-[url(../../public/images/vin_accueil.jpg)] bg-clip-border bg-cover bg-center h-72 md:h-96 md:rounded-lg mb-1 md:mb-4">
          <div className="w-full h-full flex items-center bg-white bg-opacity-40 md:bg-gradient-to-r md:from-white md:via-white md:to-transparent text-center md:text-left px-6 md:px-0 py-6 md:py-0">
              <p className="text-xl font-mono italic font-bold">
                Bienvenue chez NegoSud, votre partenaire de confiance pour l'achat de vins en ligne.
                <br/>
                <br/>
                Découvrez notre histoire et notre passion pour les vins de qualité.
              </p>
          </div>
        </div>

        <div className="flex flex-col md:flex-row items-center bg-[url(../../public/images/vin_accueil_2.jpg)] bg-clip-border bg-cover bg-center h-72 md:h-96 md:rounded-lg">
          <div className="w-full h-full flex items-center bg-white bg-opacity-40 md:bg-gradient-to-l md:from-white md:via-white md:to-transparent md:justify-end text-center md:text-right px-6 md:px-0 py-6 md:py-0">
              <p className="text-xl font-mono italic font-bold">
                Explorez nos sélections de vins exceptionnels, soigneusement choisis pour votre plaisir.
                <br/>
                <br/>
                Chez NegoSud, nous nous engageons à vous offrir une expérience de dégustation unique.
              </p>
          </div>
        </div>
      </div>
      <Footer />
    </>
  )
}
