##What can I do with Mandolin?
Slice up your test suite in equally divided chunks by swapping out nunit-console.exe for mandolin.console.exe and add an additional command line parameter in the form /slice:2of5.
  
##Why would I do that?
If you have a large suite of slow running tests like integration tests or web tests and your CI supports multiple agents, you can use Mandolin to run your suite sliced up in parallell. 

##Can't you just use <a href="http://www.plasticscm.com/infocenter/technical-articles/pnunit.aspx">PNUnit</a> for that?
If you can use PNUnit you probably should. :)

##How do I use Mandolin?
Call Mandolin just like you would nunit-console.exe with the addition of an additional parameter to indicate which slice you want to run. 
I.E.
''''
nunit-console.exe /include:Nightly,LongRunning your-test-suite.dll
''''

is replaced by
''''
mandolin.console.exe /include:Nightly,LongRunning /slice:2of5 your-test-suite.dll
''''

Notice the /slice:2of5 argument added. 