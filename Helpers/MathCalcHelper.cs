using Microsoft.Xna.Framework;

public static class MathCalcHelper
{
    public static float RotateTowards(float initialRotation, float targetRotation, float amount)
    {
        initialRotation = MathHelper.ToDegrees(initialRotation)%180;
        targetRotation = MathHelper.ToDegrees(targetRotation)%180;

        float difference = targetRotation-initialRotation;
        amount = MathHelper.Clamp(MathHelper.ToDegrees(amount), -difference, difference);

        if(difference >= 0)
        {
            initialRotation += amount;
        }
        else
        {
            initialRotation -= amount;
        }

        return MathHelper.ToRadians(initialRotation);
    }
}