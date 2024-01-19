import Header from "../../modules/layout/header";
import "./globals.css";
import Footer from "../../modules/layout/footer";

export default function Home() {
  return (
    <>
      <Header />
      <div className="max-w-screen-xl mx-auto p-4">
        <div className="my-8">
          <h1 className="text-4xl font-semibold mb-4">Bienvenue sur NegoSud</h1>
          <a href={"/listingProducts"} className="text-lg">
            Produit
          </a>
        </div>
      </div>
      <Footer />
    </>
  )
}
