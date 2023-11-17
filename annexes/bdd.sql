-- Table Commande (nouvelle table commune pour les commandes)
CREATE TABLE Commande (
    commandeID INT PRIMARY KEY,
    clientID INT, -- Peut être NULL si c'est une commande fournisseur
    fournisseurID INT, -- Peut être NULL si c'est une commande client
    employeeID INT,
    date_commande DATE,
    statut_commande VARCHAR(50),
    prix DECIMAL(10, 2),
    FOREIGN KEY (clientID) REFERENCES Client(clientID),
    FOREIGN KEY (fournisseurID) REFERENCES Fournisseur(fournisseurID)
    FOREIGN KEY (ligneCommandeID) REFERENCES LigneCommande(ligneCommandeID)
    FOREIGN KEY (statut_paiementID) REFERENCES StatutPaiement(statut_paiementID)
    FOREIGN KEY (employeeID) REFERENCES Employee(employeeID)
);

-- Table LigneCommande (nouvelle table commune pour les lignes de commandes)
CREATE TABLE LigneCommande (
    ligneCommandeID INT PRIMARY KEY,
    commandeID INT,
    articleID INT,
    quantite INT,
    prix DECIMAL(10, 2),
    FOREIGN KEY (commandeID) REFERENCES Commande(commandeID),
    FOREIGN KEY (articleID) REFERENCES Article(articleID)
);

-- Table Famille
CREATE TABLE Famille (
    familleID INT PRIMARY KEY,
    nom VARCHAR(50)
);

-- Table Fournisseur
CREATE TABLE Fournisseur (
    fournisseurID INT PRIMARY KEY,
    nom VARCHAR(100),
    adresse VARCHAR(255),
    telephone VARCHAR(100)
);

-- Table Fournisseur
CREATE TABLE Client (
    clientID INT PRIMARY KEY,
    nom VARCHAR(100),
    prenom VARCHAR(100),
    telephone VARCHAR(100),
    email VARCHAR(100),
    mot_de_passe VARCHAR(100),
    FOREIGN KEY (adresseID) REFERENCES Adresse(adresseID)
);

-- Table Fournisseur
CREATE TABLE Employee (
    employeeID INT PRIMARY KEY,
    email VARCHAR(100),
    mot_de_passe VARCHAR(100),
);

CREATE TABLE Adresse (
    adresseID INT PRIMARY KEY,
    nom VARCHAR(100),
    numero INT(100),
    rue VARCHAR(100),
    ville VARCHAR(100),
    pays VARCHAR(100),
);

-- Table Article
CREATE TABLE Article (
    articleID INT PRIMARY KEY,
    nom VARCHAR(100),
    familleID INT,
    fournisseurID INT,
    prix DECIMAL(10, 2),
    image VARCHAR(255),
    FOREIGN KEY (familleID) REFERENCES Famille(familleID),
    FOREIGN KEY (fournisseurID) REFERENCES Fournisseur(fournisseurID)
);

-- Table Stock
CREATE TABLE Stock (
    stockID INT PRIMARY KEY,
    articleID INT,
    quantite_stock INT,
    seuil_minimum INT,
    seuil_maximum INT,
    FOREIGN KEY (articleID) REFERENCES Article(articleID)
);

-- Table Paiement
CREATE TABLE Paiement (
    paymentID INT PRIMARY KEY,
    commandeID INT,
    montant DECIMAL(10, 2),
    date_paiement DATE,
    FOREIGN KEY (commandeID) REFERENCES Commande(commandeID)
    FOREIGN KEY (statut_commandeID) REFERENCES StatutCommande(statut_commandeID)
);

CREATE TABLE StatutCommande (
    statut_commandeID INT PRIMARY KEY,
    commandeID INT,
    nom VARCHAR (100),
    FOREIGN KEY (commandeID) REFERENCES Commande(commandeID)
);

CREATE TABLE StatutPaiement (
    statut_paiementID INT PRIMARY KEY,
    paymentID INT,
    nom VARCHAR (100),
    FOREIGN KEY (paymentID) REFERENCES Paiement(paymentID)
);

CREATE TABLE Panier (
    panierID INT PRIMARY KEY,
    clientID INT, -- Peut être NULL si c'est une commande fournisseur
    employeeID INT, -- Peut être NULL si c'est une commande client
    prix DECIMAL(10, 2),
    FOREIGN KEY (clientID) REFERENCES Client(clientID),
    FOREIGN KEY (employeeID) REFERENCES Employee(employeeID)
    FOREIGN KEY (lignePanierID) REFERENCES LignePanier(lignePanierID)
);

-- Table LigneCommande (nouvelle table commune pour les lignes de commandes)
CREATE TABLE LignePanier (
    lignePanierID INT,
    articleID INT,
    quantite INT,
    prix DECIMAL(10, 2),
    FOREIGN KEY (panierID) REFERENCES Panier(panierID),
    FOREIGN KEY (articleID) REFERENCES Article(articleID)
);
