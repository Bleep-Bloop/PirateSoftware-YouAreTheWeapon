namespace PSoft
{

  // An enum representing the types of weapons that can be built.
  public enum WeaponType
  {
    Sword = 0,
    Mace = 1,
    Hammer = 2,
    Axe = 3,
    Spear = 4,
    Dagger = 5
  }

  // An enum representing the size of handles used in building weapons.
  public enum HandleSize
  {
    None = 0,
    Small = 1,
    Medium = 2,
    Large = 3
  }
  
  // An enum representing the parts of a weapon.
  public enum WeaponPartType
  {
    None,
    Pommel,
    Hilt,
    Handle,
    Blade,
    // ToDo: Should this be in the assembly order? Might make it easier but do we always go in order? I don't know haha.
  }

}