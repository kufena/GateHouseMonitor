# For this particular function

Use

    dotnet lambda package

which creates the zip file in the CodeURI part of the template.  Then do

    dotnet lambda deploy-serverless --template serverless.template

to deploy this function.  If the stack exists it'll create an update set, which I think uses the same URL.  Otherwise delete the old and create a new stack,
remembering to update the Raspberry Pi with the new URL.

