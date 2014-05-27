CustomTableRegion
=================

Piranha CMS example on how to store region values in a custom separate table. This is a good approach when have lot's of pages and you have the need to search for them by values inside of the regions.

As region values are normally serialized to JSON this makes it hard to perform efficient SQL queries on the data, but with this approach a simple query can easily filter what pages that needs to be loaded.
