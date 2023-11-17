-- Table Commande (nouvelle table commune pour les commandes)
CREATE TABLE Commande (
    commandeID INT PRIMARY KEY,
    clientID INT NULL, -- Peut être NULL si c'est une commande fournisseur
    employeeID INT NULL,
    ligneCommandeID INT NOT NULL,
    date_commande DATE NOT NULL,
    statut_commandeID INT NOT NULL,
    prix DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (clientID) REFERENCES Client(clientID),
    FOREIGN KEY (ligneCommandeID) REFERENCES LigneCommande(ligneCommandeID),
    FOREIGN KEY (statut_commandeID) REFERENCES StatutCommande(statut_commandeID),
    FOREIGN KEY (employeeID) REFERENCES Employee(employeeID)
);

-- Table LigneCommande (nouvelle table commune pour les lignes de commandes)
CREATE TABLE LigneCommande (
    ligneCommandeID INT PRIMARY KEY,
    commandeID INT NOT NULL,
    articleID INT NOT NULL,
    quantite INT NOT NULL,
    prix DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (commandeID) REFERENCES Commande(commandeID),
    FOREIGN KEY (articleID) REFERENCES Article(articleID)
);

-- Table Famille
CREATE TABLE Famille (
    familleID INT PRIMARY KEY,
    nom VARCHAR(50) NOT NULL,
    articleID INT NOT NULL,
    FOREIGN KEY (articleID) REFERENCES Article(articleID)
);

-- Table Fournisseur
CREATE TABLE Fournisseur (
    fournisseurID INT PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    adresse VARCHAR(255) NOT NULL,
    telephone VARCHAR(100) NOT NULL,
    articleID INT NOT NULL,
    FOREIGN KEY (articleID) REFERENCES Article(articleID)
);

-- Table Fournisseur
CREATE TABLE Client (
    clientID INT PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    prenom VARCHAR(100) NOT NULL,
    telephone VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL,
    mot_de_passe VARCHAR(100) NOT NULL,
    adresseID INT NOT NULL,
    FOREIGN KEY (adresseID) REFERENCES Adresse(adresseID)
);

-- Table Fournisseur
CREATE TABLE Employee (
    employeeID INT PRIMARY KEY,
    email VARCHAR(100) NOT NULL,
    nom VARCHAR(100) NULL,
    prenom VARCHAR(100) NULL,
    mot_de_passe VARCHAR(100)
);

CREATE TABLE Adresse (
    adresseID INT PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    numero INT(100) NOT NULL,
    rue VARCHAR(100) NOT NULL,
    ville VARCHAR(100) NOT NULL,
    pays VARCHAR(100) NOT NULL
);

-- Table Article
CREATE TABLE Article (
    articleID INT PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    familleID INT NOT NULL,
    fournisseurID INT NOT NULL,
    prix DECIMAL(10, 2) NOT NULL,
    image VARCHAR(255) NOT NULL,
    description TEXT NULL,
    FOREIGN KEY (familleID) REFERENCES Famille(familleID),
    FOREIGN KEY (fournisseurID) REFERENCES Fournisseur(fournisseurID)
);

-- Table Stock
CREATE TABLE Stock (
    stockID INT PRIMARY KEY,
    articleID INT NOT NULL,
    quantite_article INT NOT NULL,
    seuil_minimum INT NULL,
    seuil_maximum INT NULL,
    auto_commande BOOLEAN NOT NULL,
    FOREIGN KEY (articleID) REFERENCES Article(articleID)
);

-- Table Paiement
CREATE TABLE Paiement (
    paiementID INT PRIMARY KEY,
    commandeID INT NOT NULL,
    montant DECIMAL(10, 2) NOT NULL,
    date_paiement DATE NOT NULL,
    statut_paiementID INT NOT NULL,
    FOREIGN KEY (commandeID) REFERENCES Commande(commandeID),
    FOREIGN KEY (statut_paiementID) REFERENCES StatutPaiement(statut_paiementID)
);

CREATE TABLE StatutCommande (
    statut_commandeID INT PRIMARY KEY,
    nom VARCHAR (100) NOT NULL
);

CREATE TABLE StatutPaiement (
    statut_paiementID INT PRIMARY KEY,
    nom VARCHAR (100) NOT NULL
);

CREATE TABLE Panier (
    panierID INT PRIMARY KEY,
    clientID INT NULL, -- Peut être NULL si c'est une commande fournisseur
    employeeID INT NULL, -- Peut être NULL si c'est une commande client
    prix DECIMAL(10, 2) NOT NULL,
    lignePanierID INT NOT NULL,
    FOREIGN KEY (clientID) REFERENCES Client(clientID),
    FOREIGN KEY (employeeID) REFERENCES Employee(employeeID),
    FOREIGN KEY (lignePanierID) REFERENCES LignePanier(lignePanierID)
);

-- Table LigneCommande (nouvelle table commune pour les lignes de commandes)
CREATE TABLE LignePanier (
    lignePanierID INT NOT NULL,
    articleID INT NOT NULL,
    quantite INT NOT NULL,
    prix DECIMAL(10, 2) NOT NULL,
    panierID INT NOT NULL,
    FOREIGN KEY (panierID) REFERENCES Panier(panierID),
    FOREIGN KEY (articleID) REFERENCES Article(articleID)
);
