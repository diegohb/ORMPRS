ORMPRS
======

An ORM-agnostic implementation for data access based on the [EFPRS project](https://github.com/huyrua/efprs).  More information about EFPRS can be found at [Huy Nguyen](http://huyrua.wordpress.com/2011/04/13/entity-framework-4-poco-repository-and-specification-pattern-upgraded-to-ef-4-1/).

This is intended to be IoC-ready following principles of the [onion architecture](http://jeffreypalermo.com/blog/the-onion-architecture-part-1/).

As a proof-of-concept, the plan is to add 2 sample northwind web applications that are implemented with OpenAccess and EF respectively. 

Par of the purpose is to test out differences with the two ORM technologies that are detailed here: http://www.telerik.com/products/orm/getting-started/openaccess-vs-entity-framework.aspx.

Patterns
======
P = POCO
R = Repository
S = Specification
