## BetterTesla

**BetterTesla** is a plugin for SCP: Secret Laboratory using EXILED. It enhances Tesla Gate behavior with dynamic and customizable mechanics like **item-based immunity**, **chaotic Tesla surges**, and **overcharged blackout sequences** — designed to create more immersive and unpredictable rounds.

[![downloads](https://img.shields.io/github/downloads/TheKolo12/Better-Tesla/total?style=for-the-badge\&logo=tesla\&color=%232875B2)](https://github.com/TheKolo12/Better-Tesla/releases/latest)
![Latest](https://img.shields.io/github/v/release/TheKolo12/Better-Tesla?style=for-the-badge\&label=Latest%20Release\&color=%23D91656)

---

### **What It Does**

* **Advanced Tesla Logic:**

  * Players carrying specific items (configurable) will **not trigger Tesla gates**.
  * Customizable hints inform players why they weren't shocked.

* **Crazy Tesla Mode:**

  * Tesla gates enter a **chaotic activation loop** when triggered once.
  * Plays custom **CASSIE announcements** and triggers all Tesla gates at random intervals.
  * Can be started automatically or manually via RemoteAdmin:
    `forceevent crazy`

* **Overcharged Tesla Events:**

  * Each Tesla activation is tracked over time.
  * If the number of total activations exceeds a threshold:

    * A **blackout is triggered** across all zones.
    * All **doors are locked and closed** temporarily.
    * Plays a custom **CASSIE message** for immersion.
  * Also triggerable manually with:
    `forceevent overcharged`

* Includes **event reset on round end**, logging, and command overrides for debugging or RP use.

---

### Requirements

* **EXILED API v9.6.0 or newer**
* SCP\:SL server with plugin support

### Installation

1. Place the compiled `.dll` file into your `EXILED/Plugins` folder.
2. Run the server once to generate the config file.
3. Customize your config as desired.
4. Restart the server — done!


