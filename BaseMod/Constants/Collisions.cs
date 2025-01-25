using nkast.Aether.Physics2D.Dynamics;

namespace Base.Constants;

//TODO Temporary until better system is in place
public static class Collisions {

    public static class Cats {
        public const Category Player = Category.Cat1;
        public const Category Friendly = Category.Cat2;
        public const Category Hostile = Category.Cat3;
        public const Category Projectiles = Category.Cat4;

        public const Category PlayerAligned = Player | Friendly;
    }

    public static class CollidesWith {
        public const Category Player = Cats.Friendly | Cats.Hostile | Cats.Projectiles;
        public const Category Friendly = Cats.Friendly | Cats.Hostile | Cats.Projectiles;
        public const Category Hostile = Cats.Friendly | Cats.Hostile | Cats.Projectiles;
        
        public const Category FriendlyProjectiles = Cats.Hostile;
        public const Category HostileProjectiles = Cats.Friendly;
    }

    public static Category With(this Category thisCat, Category category, bool shouldInclude) {
        return thisCat | (shouldInclude ? category : Category.None);
    }
    
}