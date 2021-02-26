# Change Log
## Version 0.1 - 2/26/2021
- Added cards and have them highlighting and being created with a prefab.
- Started columns.
- Added the deck and dealing functionality.
- Added a deck data scriptable object which hold the deck name and card sprites.
- Refactored the code base.

## Dev Log 1 - Justin D. Selsor
I added cards into the project but still need to add the dragging and dropping to them. This will be done when the columns are working. The I started work on the columns. The columns are separated into 4 kinds: **Basic Columns**, **Tableau Columns**, **Waste Columns**, and **Foundation Columns**.
- **Basic Columns** are really just a list of cards but should be useful to have something like a dragging column which cards can be added to when the player is dragging a set of cards. This is the parent for all other columns.
- **Tableau Columns** are the most complex types of columns. This represents the tableau in a game of Solitaire and there should be rules when trying to add cards to it. Cards should only be able to enter from the **Waste Column**, other **Tableau Columns**, and **Foundation Columns**. There are seven **Tableau Columns.**
- **Waste Columns** are the columns where cards from the rest of the deck go after setting up the tableau. Cards should only be able to enter this column from the deck and once the deck is empty then the waste should be added back to the deck in the same order. The will only be one **Waste Column**.
- **Foundation Columns** are where sorted cards go. They are built from bottom up (ace, two, three, ..., jack, queen, king) and must be all of the same suit. Card may only enter this column if they are coming from the **Waste** or **Tableau Columns**. There are four **Foundation Columns**.

I then added the deck into the game along with a scriptable object to represent information about the deck. This scriptable object which I call *Deck Data* holds the name of the deck, the card back sprite, and a list of all the card fronts. This should allow the team to create many different kinds of decks in the feature and should aid in any DLC plans using Streamable Assets? I need to look into how to do DLC in Unity. I should also think about converting the game assets into Streamable Assets but that will be an issue for future me and the team. With the deck I just set up the creation of the deck and instantiation of Cards, deck shuffling using the Fisher-Yates method, and dealing the top card of the deck.
