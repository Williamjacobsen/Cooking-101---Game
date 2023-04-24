const express = require("express");
const cors = require("cors");
const bodyParser = require("body-parser");
const mysql = require("mysql2");
const os = require("os");
const bcrypt = require("bcrypt");

const app = express();

const saltRounds = 10; // work factor / time spendt encrypting

// middleware
// cors specifics allowed requests
app.use(
  cors({
    origin: ["*"],
    method: ["GET", "POST"],
    credentials: true,
  })
);
// to be able to parse json
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

require("dotenv").config(); // so that password is not written in the code
const db = mysql.createConnection({
  user: os.type() === "Linux" ? "admin" : "root",
  host: "localhost",
  password: process.env.PASSWORD,
  database: "cooking101",
});

/**
 * @param {string} username
 * @returns {boolean}
 * checks if account exists by username
 */
const DoesAccountExist = async (username) => {
  return new Promise((resolve, reject) => {
    // waits for completion
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
  db.query(
    "SELECT * FROM `cooking101`.accounts WHERE username = ?",
    [req.body.username],
    (err, result) => {
      if (err) {
        console.log(err);
        res.send("Account doesn't exist");
        return;
      }
      if (result[0]) {
        bcrypt.compare(
          req.body.password,
          result[0]["password"],
          (err, isSamePassword) => {
            if (isSamePassword) {
              res.send("Success");
            }
          }
        );
      } else {
        res.send("Account doesn't exist");
      }
    }
  );
});

app.post("/signup", async (req, res) => {
  if (await DoesAccountExist(req.body.username)) {
    res.send("Account already exists");
    return;
  }

  bcrypt.hash(req.body.password, saltRounds, (err, hashedPassword) => {
    if (err) {
      res.send(err);
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
