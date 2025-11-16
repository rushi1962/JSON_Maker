using System;

//Defining an attribute. If a class has this attribute, It'll be shown in JSON maker window.
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class JSONConvertableAttribute: Attribute
{
    
}
