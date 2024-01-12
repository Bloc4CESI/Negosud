import Header from "../../../modules/layout/header";
import Footer from "../../../modules/layout/footer";
import '../globals.css';

const HomePage = () => {
  return (
    <div>
      <Header />
      <main className="max-w-screen-xl mx-auto p-4">
        <section className="my-8">
          <h1 className="text-4xl font-semibold mb-4">Bienvenue sur NegoSud</h1>
          <a href={"/product"} className="text-lg">
            Produit
          </a>
        </section>
      </main>

      <Footer />
    </div>
  );
};

export default HomePage;
