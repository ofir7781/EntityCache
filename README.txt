Hello,
Little explanation about what i have built:
i implemented the Entity Cache in a generic way with a commitment that 
all the generic types must be from type Entity. Moreover, i built a ststic class (named "ObjectAndDictionaryTwoWayConverter")
which is a service class that convert object to a Dictionary and Dictionary to object. In my program, i gave an example of
working with text files but as you can see from the code, because of the interface "IRepositoryProvider" it will help us to
work easily with any other persistent doucment-based repository provider in the future. Same for the static class. In order 
to show what i have built, i wrote a mini Console Ui that we can work with:
This Ui specifically work with school generic class that works with type Student class(which is our entity in the Ui) as a user, 
but you can see it will work on other users and entitys if we will decide to add in the future.
The Console Ui mainly give us user experience of defining new School users in our project and perform operations on them according to 
what is required. In addition, the program check perfection of various situations and Ui handle exceptions that thrown from our Entity Cache.
