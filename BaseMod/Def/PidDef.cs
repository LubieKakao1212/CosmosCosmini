namespace Base.Weapons;

public class PidDef {
    
}

public class Pid(float p, float i, float d, float k) {

    private float _integral = 0f;
    private float _state = 0f;

    public void Reset() {
        _integral = 0f;
        _state = 0f;
    }

    public float Evaluate(float error) {
        _integral = error * 1 + _integral;//Math.Clamp(error + _integral, -1f, 1f);
        var derivative = (_state - error) / 1;
        _state = error;

        return
            (p * error + i * _integral +
            d * derivative) * k; //Math.Clamp(p * error + i * _integral + d * derivative, -10f, 10f);
    }
    
}