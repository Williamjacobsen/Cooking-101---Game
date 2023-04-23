const express = require("express");
const cors = require("cors");
const bodyParser = require("body-parser");
const mysql = require("mysql2");
const os = require("os");
const bcrypt = require("bcrypt");

const app = express();

const saltRounds = 10;

app.use(
  cors({
    origin: ["*"],
    method: ["GET", "POST"],
    credentials: true,
  })
);
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));

/*
  -- Database --
    > cooking101
    -- Tables --
      > accounts
      -- Attributes --
        > id
        > username
        > password
*/

require("dotenv").config();
const db = mysql.createConnection({
  user: os.type() === "Linux" ? "admin" : "root",
  host: "localhost",
  password: process.env.PASSWORD,
  database: "cooking101",
});

/**
 *
 * @param {string} username
 * @returns {boolean}
 * checks if account exists by username
 */
const DoesAccountExist = async (username) => {
  return new Promise((resolve, reject) => {
    db.query(
      "SELECT * FROM `cooking101`.accounts WHERE username = ?",
      [username],
      (err, result) => {
        if (err) {
          return reject(new Error(false));
        }
        if (result[0]) {
          return resolve(true);
        }
        return reject(new Error(false));
      }
    );
  })
    .then(() => {
      return true;
    })
    .catch(() => {
      return false;
    });
};

app.post("/login", (req, res) => {
  bcrypt.hash(req.body.password, saltRounds, (err, hashedPassword) => {
    if (err) {
      res.send("Couldn't create account");
      console.log(err);
      return;
    }

    db.query(
      "SELECT * FROM `cooking101`.accounts WHERE username = ? AND password = ?",
      [req.body.username, hashedPassword],
      (err, result) => {
        if (err) {
          console.log(err);
          res.send("Account doesn't exist");
          return;
        }
        res.send("Success");
      }
    );
  });
});

app.post("/signup", async (req, res) => {
  if (await DoesAccountExist(req.body.username)) {
    res.send("Account already exists");
    return;
  }

  bcrypt.hash(req.body.password, saltRounds, (err, hashedPassword) => {
    if (err) {
      res.send("Couldn't create account");
      console.log(err);
      return;
    }

    db.query(
      "INSERT INTO `cooking101`.accounts (username, password) VALUES (?, ?)",
      [req.body.username, hashedPassword],
      (err, result) => {
        if (err) {
          console.log(err);
          res.send("Couldn't create account");
          return;
        }
        res.send("Success");
      }
    );
  });
});

app.listen(5000, () => console.log(`Server listening on port 5000...`));
