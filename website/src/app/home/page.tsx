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
          <p className="text-lg">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque id lectus non ligula
            tempor facilisis nec eu ipsum. Integer cursus metus quis erat fermentum, a dapibus
            augue ultricies.
          </p>
        </section>
      </main>

      <Footer />
    </div>
  );
};

export default HomePage;
